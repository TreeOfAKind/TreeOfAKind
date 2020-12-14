import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { TreeCreateRequest } from './tree-create-request.model';
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

  createTree(request: TreeCreateRequest) {
    return this.httpClient.post(`${this.url}CreateTree`, request);
  }
}
