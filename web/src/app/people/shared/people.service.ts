import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { FileResponse } from './file-response.model';
import { PersonForm } from './person-form.model';
import { PersonUpdate } from './person-update.model';
import { RemovePersonsFileRequest } from './remove-persons-file.model';

@Injectable({
  providedIn: 'root'
})
export class PeopleService {
  url = `${environment.apiUrl}People`;
  filesUrl = `${environment.apiUrl}PersonsFiles`;

  constructor(
    private httpClient: HttpClient
  ) { }

  addPerson(person: PersonForm) {
    return this.httpClient.post(`${this.url}/AddPerson`, person);
  }

  removePerson(personId: string, treeId: string) {
    return this.httpClient.post(`${this.url}/RemovePerson`, { treeId: treeId, personId: personId});
  }

  updatePerson(person: PersonUpdate) {
    return this.httpClient.post(`${this.url}/UpdatePerson`, person, { observe: 'response' });
  }

  addPersonsFile(request: FormData) {
    return this.httpClient.post(`${this.filesUrl}/AddPersonsFile`, request);
  }

  removePersonsFile(removeRequest: RemovePersonsFileRequest) {
    return this.httpClient.post(`${this.filesUrl}/RemovePersonsFile`, removeRequest);
  }

  changePersonsMainPhoto(request: FormData): Observable<FileResponse> {
    return this.httpClient.post<FileResponse>(`${this.filesUrl}/AddOrChangePersonsMainPhoto`, request);
  }

}
