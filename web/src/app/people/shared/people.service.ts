import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { PersonForm } from './person-form.model';

@Injectable({
  providedIn: 'root'
})
export class PeopleService {
  url = `${environment.apiUrl}People`;

  constructor(
    private httpClient: HttpClient
  ) { }

  addPerson(person: PersonForm) {
    return this.httpClient.post(`${this.url}/AddPerson`, person);
  }

  removePerson(personId: string, treeId: string) {
    return this.httpClient.post(`${this.url}/RemovePerson`, { treeId: treeId, personId: personId});
  }

}
