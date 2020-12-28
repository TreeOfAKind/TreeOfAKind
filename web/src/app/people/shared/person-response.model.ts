import { FileResponse } from "./file-response.model";

export interface PersonResponse {
  id: string,
  treeId: string,
  name: string,
  lastName: string,
  gender: string,
  birthDate: Date,
  deathDate: Date,
  description: string,
  biography: string,
  mother: string,
  father: string,
  spouse: string,
  mainPhoto: FileResponse,
  files: FileResponse[]
}
