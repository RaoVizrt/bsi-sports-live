import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Team, TeamCreate, TeamUpdate } from '../models/team.model';

@Injectable({
  providedIn: 'root'
})
export class TeamService {
  private apiUrl = 'http://localhost:5258/api/teams';

  constructor(private http: HttpClient) {}

  getTeams(): Observable<Team[]> {
    return this.http.get<Team[]>(this.apiUrl);
  }

  getTeamById(id: number): Observable<Team> {
    return this.http.get<Team>(`${this.apiUrl}/${id}`);
  }

  createTeam(team: TeamCreate): Observable<Team> {
    return this.http.post<Team>(this.apiUrl, team);
  }

  updateTeam(id: number, team: TeamUpdate): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, team);
  }

  deleteTeam(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

    addPlayerToTeam(teamId: number, playerId: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/${teamId}/players/${playerId}`, {});
  }

  removePlayerFromTeam(teamId: number, playerId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${teamId}/players/${playerId}`);
  }
}