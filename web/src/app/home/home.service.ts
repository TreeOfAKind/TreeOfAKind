import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment'
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HomeService {

  constructor(
    private httpClient: HttpClient
  ) { }

  ping(text: string): Observable<string> {
    return this.httpClient.get<string>(`${environment.apiUrl}ping/${text}`);
  }
}
