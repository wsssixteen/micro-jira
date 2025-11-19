import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';

export interface AppConfig {
  production: boolean;
  apiUrl: string;
}

@Injectable({
  providedIn: 'root'
})
export class ConfigService {
  private config: AppConfig | null = null;

  constructor(private http: HttpClient) {}

  async loadConfig(): Promise<AppConfig> {
    if (this.config) {
      return this.config;
    }

    try {
      this.config = await firstValueFrom(this.http.get<AppConfig>('/config.json'));
      return this.config;
    } catch (error) {
      console.error('Failed to load config.json, using defaults:', error);
      // Fallback to defaults
      this.config = {
        production: false,
        apiUrl: 'http://localhost:5215'
      };
      return this.config;
    }
  }

  getConfig(): AppConfig {
    if (!this.config) {
      throw new Error('Config not loaded. Call loadConfig() first.');
    }
    return this.config;
  }

  getApiUrl(): string {
    return this.getConfig().apiUrl;
  }

  isProduction(): boolean {
    return this.getConfig().production;
  }
}
