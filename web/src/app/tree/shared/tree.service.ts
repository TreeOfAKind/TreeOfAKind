import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { TreesListResponse } from './trees-list-response.model';

@Injectable({
  providedIn: 'root'
})
export class TreeService {
  url = `${environment.apiUrl}Tree/`;

  constructor(
    private httpClient: HttpClient
  ) { }

  getMyTrees(): Observable<TreesListResponse> {
    return this.httpClient.post<TreesListResponse>(`${this.url}GetMyTrees`, {});
  }
}
