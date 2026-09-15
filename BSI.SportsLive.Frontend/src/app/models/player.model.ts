export interface TeamMembership {
  teamId: number;
  teamName: string;
  joinedDate: string;
  leftDate: string | null;
}

export interface Player {
  id: number;
  fullName: string;
  broadcastName: string;
  shirtNumber: string;
  photoUrl: string | null;
  currentTeams: TeamMembership[];
  pastTeams: TeamMembership[];
}

export interface PlayerCreate {
  fullName: string;
  broadcastName?: string;
  shirtNumber?: string;
  photoUrl?: string;
}

export interface PlayerUpdate extends PlayerCreate {
  id: number;
}