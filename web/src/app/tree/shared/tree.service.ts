import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { TreeCreateRequest } from './tree-create-request.model';
import { TreeStats } from './tree-stats.model';
import { Tree } from './tree.model';
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

  getTree(id: string): Observable<Tree> {
    return this.httpClient.post<Tree>(`${this.url}GetTree`, { treeId: id });
  }

  createTree(request: TreeCreateRequest) {
    return this.httpClient.post(`${this.url}CreateTree`, request);
  }

  changePhoto(request: FormData) {
    return this.httpClient.post(`${this.url}AddOrChangeTreePhoto`, request);
  }

  addOwner(treeId: string, email: string) {
    return this.httpClient.post(`${this.url}AddTreeOwner`,
    {
      treeId: treeId,
      invitedUserEmail: email
    });
  }

  removeOwner(treeId: string, userId?: string) {
    return this.httpClient.post(`${this.url}RemoveTreeOwner`,
    {
      treeId: treeId,
      removedUserId: userId
    });
  }

  mergeTrees(firstTreeId: string, secondTreeId: string) {
    return this.httpClient.post(`${this.url}MergeTrees`,
    {
      firstTreeId: firstTreeId,
      secondTreeId: secondTreeId
    });
  }
  
  getTreeStatistics(treeId: string): Observable<TreeStats> {
    return this.httpClient.post<TreeStats>(`${this.url}GetTreeStatistics`, { treeId: treeId });
  }
}
