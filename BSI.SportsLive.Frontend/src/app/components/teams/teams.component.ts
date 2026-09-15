import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TeamService } from '../../services/team.service';
import { PlayerService } from '../../services/player.service';
import { UploadService } from '../../services/upload.service';
import { Team, TeamCreate, PlayerSummary } from '../../models/team.model';
import { Player, PlayerCreate } from '../../models/player.model';

@Component({
  selector: 'app-teams',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './teams.component.html',
  styleUrls: ['./teams.component.css']
})
export class TeamsComponent implements OnInit {
  teams: Team[] = [];
  isLoading = false;
  errorMessage = '';

  // Create team modal
  showTeamModal = false;
  newTeam: TeamCreate = { name: '', shortName: '', sportType: 'Cricket' };

  // Manage players modal
  showPlayersModal = false;
  selectedTeam: Team | null = null;

  // Two sub-modes inside the players modal
  playerModalMode: 'list' | 'addExisting' | 'createNew' = 'list';

  // Add Existing Player (global pool search)
  allPlayers: Player[] = [];
  playerSearchTerm = '';
  isSearchingPlayers = false;

  // Create New Player
  newPlayer: PlayerCreate = { fullName: '', broadcastName: '', shirtNumber: '' };
  selectedPhotoFile: File | null = null;
  photoPreviewUrl: string | null = null;
  photoError = '';
  isUploadingPhoto = false;

  constructor(
    private teamService: TeamService,
    private playerService: PlayerService,
    private uploadService: UploadService
  ) {}

  ngOnInit(): void {
    this.loadTeams();
  }

  loadTeams(): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.teamService.getTeams().subscribe({
      next: (data) => { this.teams = data; this.isLoading = false; },
      error: (err) => {
        console.error('Error fetching teams:', err);
        this.errorMessage = 'Failed to load teams.';
        this.isLoading = false;
      }
    });
  }

  // ---- Create Team ----
  openCreateTeamModal(): void {
    this.newTeam = { name: '', shortName: '', sportType: 'Cricket' };
    this.showTeamModal = true;
  }

  closeTeamModal(): void {
    this.showTeamModal = false;
  }

  createTeam(): void {
    this.teamService.createTeam(this.newTeam).subscribe({
      next: () => { this.showTeamModal = false; this.loadTeams(); },
      error: (err) => {
        console.error('Error creating team:', err);
        alert('Failed to create team. Please check details.');
      }
    });
  }

  deleteTeam(team: Team): void {
    if (!confirm(`Delete team "${team.name}"? This cannot be undone.`)) return;
    this.teamService.deleteTeam(team.id).subscribe({
      next: () => this.loadTeams(),
      error: (err) => { console.error('Error deleting team:', err); alert('Failed to delete team.'); }
    });
  }

  // ---- Manage Players Modal ----
  openPlayersModal(team: Team): void {
    this.selectedTeam = team;
    this.playerModalMode = 'list';
    this.showPlayersModal = true;
  }

  closePlayersModal(): void {
    this.showPlayersModal = false;
    this.selectedTeam = null;
    this.resetCreatePlayerForm();
  }

  refreshSelectedTeam(): void {
    if (!this.selectedTeam) return;
    this.teamService.getTeamById(this.selectedTeam.id).subscribe({
      next: (team) => {
        this.selectedTeam = team;
        const idx = this.teams.findIndex(t => t.id === team.id);
        if (idx > -1) this.teams[idx] = team;
      },
      error: (err) => console.error('Error refreshing team:', err)
    });
  }

  removePlayerFromTeam(player: PlayerSummary): void {
    if (!this.selectedTeam) return;
    if (!confirm(`Remove ${player.broadcastName} from this team?`)) return;
    this.teamService.removePlayerFromTeam(this.selectedTeam.id, player.id).subscribe({
      next: () => this.refreshSelectedTeam(),
      error: (err) => { console.error('Error removing player:', err); alert('Failed to remove player.'); }
    });
  }

  // ---- Add Existing Player (global pool) ----
  openAddExistingMode(): void {
    this.playerModalMode = 'addExisting';
    this.playerSearchTerm = '';
    this.loadAllPlayers();
  }

  loadAllPlayers(): void {
    this.isSearchingPlayers = true;
    this.playerService.getPlayers().subscribe({
      next: (data) => { this.allPlayers = data; this.isSearchingPlayers = false; },
      error: (err) => {
        console.error('Error fetching players:', err);
        this.isSearchingPlayers = false;
      }
    });
  }

  get filteredPlayers(): Player[] {
    const term = this.playerSearchTerm.trim().toLowerCase();
    if (!term) return this.allPlayers;
    return this.allPlayers.filter(p =>
      p.fullName.toLowerCase().includes(term) ||
      p.broadcastName.toLowerCase().includes(term)
    );
  }

  isPlayerInCurrentTeam(player: Player): boolean {
    if (!this.selectedTeam) return false;
    return player.currentTeams.some(t => t.teamId === this.selectedTeam!.id);
  }

  addExistingPlayer(player: Player): void {
    if (!this.selectedTeam) return;
    this.teamService.addPlayerToTeam(this.selectedTeam.id, player.id).subscribe({
      next: () => {
        this.refreshSelectedTeam();
        this.loadAllPlayers(); // badge update ho jaye "already in team"
      },
      error: (err) => {
        console.error('Error adding player:', err);
        alert(err?.error?.message || err?.error || 'Failed to add player.');
      }
    });
  }

  // ---- Create New Player ----
  openCreateNewMode(): void {
    this.playerModalMode = 'createNew';
    this.resetCreatePlayerForm();
  }

  resetCreatePlayerForm(): void {
    this.newPlayer = { fullName: '', broadcastName: '', shirtNumber: '' };
    this.selectedPhotoFile = null;
    this.photoPreviewUrl = null;
    this.photoError = '';
  }

  onPhotoSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];
    if (!file) return;

    const error = this.uploadService.validateFile(file);
    if (error) {
      this.photoError = error;
      this.selectedPhotoFile = null;
      this.photoPreviewUrl = null;
      return;
    }

    this.photoError = '';
    this.selectedPhotoFile = file;
    this.photoPreviewUrl = URL.createObjectURL(file);
  }

  createNewPlayer(): void {
    if (!this.selectedTeam) return;

    if (!this.newPlayer.fullName.trim()) {
      alert('Player full name is required.');
      return;
    }

    if (this.selectedPhotoFile) {
      this.isUploadingPhoto = true;
      this.uploadService.uploadPlayerPhoto(this.selectedPhotoFile).subscribe({
        next: (res) => {
          this.newPlayer.photoUrl = res.url;
          this.isUploadingPhoto = false;
          this.savePlayerAndAddToTeam();
        },
        error: (err) => {
          console.error('Error uploading photo:', err);
          this.isUploadingPhoto = false;
          alert(err?.error?.message || 'Failed to upload photo.');
        }
      });
    } else {
      this.savePlayerAndAddToTeam();
    }
  }

  private savePlayerAndAddToTeam(): void {
    if (!this.selectedTeam) return;
    this.playerService.createPlayer(this.newPlayer).subscribe({
      next: (createdPlayer) => {
        this.teamService.addPlayerToTeam(this.selectedTeam!.id, createdPlayer.id).subscribe({
          next: () => {
            this.playerModalMode = 'list';
            this.resetCreatePlayerForm();
            this.refreshSelectedTeam();
          },
          error: (err) => {
            console.error('Error adding new player to team:', err);
            alert('Player created but failed to add to team.');
          }
        });
      },
      error: (err) => {
        console.error('Error creating player:', err);
        alert('Failed to create player.');
      }
    });
  }
}