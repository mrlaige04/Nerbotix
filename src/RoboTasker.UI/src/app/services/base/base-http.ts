import {Inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {apiConfig, ApiConfig} from '../../config/http.config';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BaseHttp {
  private readonly baseUrl: string;
  constructor(private http: HttpClient, @Inject(apiConfig) apiConfig: ApiConfig) {
    this.baseUrl = apiConfig.url;
  }

  public post<Req, Res>(url: string, data: Req, params = {}, headers = {}): Observable<Res> {
    const fullUrl = `${this.baseUrl}/${url}`;
    return this.http.post<Res>(fullUrl, data, { params, headers });
  }

  public get<Res>(url: string, params = {}, headers = {}): Observable<Res> {
    const fullUrl = `${this.baseUrl}/${url}`;
    return this.http.get<Res>(fullUrl, { params, headers });
  }

  public put<Req, Res>(url: string, data: Req, params = {}, headers = {}): Observable<Res> {
    const fullUrl = `${this.baseUrl}/${url}`;
    return this.http.put<Res>(fullUrl, data, { params, headers });
  }

  public delete<Res>(url: string, params = {}, headers = {}): Observable<Res> {
    const fullUrl = `${this.baseUrl}/${url}`;
    return this.http.delete<Res>(fullUrl, { params, headers });
  }
}
