export interface PlayerSummary {
  id: number;
  broadcastName: string;
  shirtNumber: string;
  photoUrl: string | null;
}

export interface Team {
  id: number;
  name: string;
  shortName: string;
  sportType: string;
  players: PlayerSummary[];
}

export interface TeamCreate {
  name: string;
  shortName: string;
  sportType: string;
}

export interface TeamUpdate extends TeamCreate {
  id: number;
}