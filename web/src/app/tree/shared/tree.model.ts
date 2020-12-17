import { PersonForm } from "src/app/people/shared/person-form.model";

export interface Tree {
  treeId: string,
  treeName: string,
  photoUri: string,
  people: PersonForm[]
}
