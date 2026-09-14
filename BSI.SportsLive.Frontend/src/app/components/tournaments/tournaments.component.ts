import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TournamentService } from '../../services/tournament.service';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-tournaments',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './tournaments.component.html',
  styleUrls: ['./tournaments.component.css']
})
export class TournamentsComponent implements OnInit {
  tournaments: any[] = [];
  showModal: boolean = false;
  isLoading: boolean = false;
  errorMessage: string = '';

  newTournament = {
    name: '',
    sport: 'Cricket',
    stageFormat: 'Knockout',
    participantsCount: 8,
    format: 'T20',
    oversPerInnings: 20
  };

  availableFormats: string[] = ['T20', 'ODI', 'Test', '10-Over'];

  constructor(
    private tournamentService: TournamentService, 
    private router: Router,
    private cdr: ChangeDetectorRef // ChangeDetectorRef inject kiya gaya hai
  ) {}

  ngOnInit(): void {
    this.loadTournaments();
  }

  onSportChange(): void {
    if (this.newTournament.sport === 'Cricket') {
      this.availableFormats = ['T20', 'ODI', 'Test', '10-Over'];
      this.newTournament.format = 'T20';
      this.newTournament.oversPerInnings = 20;
    } else if (this.newTournament.sport === 'Football') {
      this.availableFormats = ['Standard (90 mins)', 'Friendly', '7-a-side'];
      this.newTournament.format = 'Standard (90 mins)';
      this.newTournament.oversPerInnings = 0;
    } else {
      this.availableFormats = ['Standard'];
      this.newTournament.format = 'Standard';
      this.newTournament.oversPerInnings = 0;
    }
  }

  loadTournaments(): void {
    this.isLoading = true;
    this.errorMessage = '';
    
    this.tournamentService.getTournaments().subscribe({
      next: (data) => {
        console.log('Tournaments fetched successfully:', data);
        this.tournaments = Array.isArray(data) ? data : [];
        this.isLoading = false;
        this.cdr.detectChanges(); // UI ko foran refresh karne ke liye force trigger
      },
      error: (err: any) => {
        console.error('Error fetching tournaments:', err);
        this.errorMessage = 'Failed to load tournaments.';
        this.isLoading = false;
        this.tournaments = [];
        this.cdr.detectChanges();
      }
    });
  }

  openCreateModal(): void {
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
  }

  createTournament(): void {
    this.tournamentService.createTournament(this.newTournament).subscribe({
      next: (res: any) => {
        console.log('Tournament created successfully', res);
        this.showModal = false; 
        
        // Agar response mein tournament ki ID aa rahi hai, toh foran uske manage page par redirect kar dein
        if (res && res.id) {
          this.router.navigate(['/tournaments', res.id]);
        } else {
          this.loadTournaments();
        }
        
        // Form reset karna
        this.newTournament = { 
          name: '', 
          sport: 'Cricket', 
          stageFormat: 'Knockout', 
          format: 'T20', 
          participantsCount: 8,
          oversPerInnings: 20 
        };
        
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        console.error('Error creating tournament:', err);
        alert('Failed to create tournament. Please check details.');
      }
    });
  }

  logout(): void {
    localStorage.clear();
    this.router.navigate(['/login']);
  }
}