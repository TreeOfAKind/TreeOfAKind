import { PersonResponse } from "src/app/people/shared/person-response.model";
import { Owner } from "./owner.model";

export interface Tree {
  treeId: string,
  treeName: string,
  photoUri: string,
  people: PersonResponse[],
  owners: Owner[]
}
