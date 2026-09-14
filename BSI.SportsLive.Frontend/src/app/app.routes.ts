import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { TournamentsComponent } from './components/tournaments/tournaments.component';
import { TournamentDetailComponent } from './components/tournament-detail/tournament-detail.component'; // Yeh import lazmi karein

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'tournaments', component: TournamentsComponent },
  { path: 'tournaments/:id', component: TournamentDetailComponent }, // Yeh route add karna hai
  { path: '**', redirectTo: 'dashboard' }
];