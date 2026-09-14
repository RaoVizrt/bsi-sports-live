import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { TournamentService } from '../../services/tournament.service';

@Component({
  selector: 'app-tournament-detail',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './tournament-detail.component.html',
  styleUrls: ['./tournament-detail.component.css']
})
export class TournamentDetailComponent implements OnInit {
  tournamentId!: number;
  tournament: any = null;
  isLoading: boolean = true;
  errorMessage: string = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private tournamentService: TournamentService,
    private cdr: ChangeDetectorRef // Yeh inject kiya gaya hai
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const idParam = params.get('id');
      if (idParam) {
        this.tournamentId = +idParam;
        this.loadTournamentDetails(this.tournamentId);
      }
    });
  }

  loadTournamentDetails(id: number): void {
    this.isLoading = true;
    this.tournamentService.getTournamentById(id).subscribe({
      next: (data) => {
        console.log('Tournament detail fetched successfully:', data);
        this.tournament = data;
        this.isLoading = false;
        this.cdr.detectChanges(); // UI ko force refresh karne ke liye
      },
      error: (err) => {
        console.error('Error fetching tournament details:', err);
        this.errorMessage = 'Failed to load tournament details.';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }
}