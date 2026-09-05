import { CodePage } from './compte.model';

export interface CompteAdminDTO {
  idCompte: number;
  email: string;
  estAdmin: boolean;
  pages: CodePage[];
  dateCreation: string;
  dateDerniereConnexion: string | null;
}

export interface UpdateAccesPagesDTO {
  pages: CodePage[];
}
