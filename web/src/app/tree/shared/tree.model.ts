import { PersonResponse } from "src/app/people/shared/person-response.model";

export interface Tree {
  treeId: string,
  treeName: string,
  photoUri: string,
  people: PersonResponse[]
}
