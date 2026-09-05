export interface CompteDTO {
  idCompte: number;
  email: string;
  doitChangerMotDePasse: boolean;
  dateCreation: string;
  dateDerniereConnexion: string | null;
}

export interface LoginResponseDTO {
  idCompte: number;
  email: string;
  doitChangerMotDePasse: boolean;
}
