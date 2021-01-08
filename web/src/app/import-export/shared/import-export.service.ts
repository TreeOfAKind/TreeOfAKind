import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ImportExportService {
  url = `${environment.apiUrl}Tree/`;

  constructor(
    private httpClient: HttpClient
  ) { }

  createTreeFromFile(request: FormData) {
    return this.httpClient.post(`${this.url}/CreateTreeFromFile`, request);
  }

  getTreeFileExport(treeId: string){
    return this.httpClient.post(`${this.url}/GetTreeFileExport`,
      { treeId: treeId },
      {
        headers: { 'Accept': 'text/xml' },
        responseType: 'text',
        observe: 'response'
      });
  }

}
