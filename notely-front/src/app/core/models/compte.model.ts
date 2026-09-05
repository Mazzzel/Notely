export type CodePage = 'cours' | 'salle';

export interface CompteDTO {
  idCompte: number;
  email: string;
  doitChangerMotDePasse: boolean;
  estAdmin: boolean;
  pages: CodePage[];
  dateCreation: string;
  dateDerniereConnexion: string | null;
}

export interface LoginResponseDTO {
  idCompte: number;
  email: string;
  doitChangerMotDePasse: boolean;
  estAdmin: boolean;
  pages: CodePage[];
  token: string;
}
