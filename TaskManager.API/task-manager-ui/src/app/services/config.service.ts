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
    if (this.config) return this.config;

    try {
      // Try local fonfig first
      this.config = await firstValueFrom(this.http.get<AppConfig>('/config.json'));
    } catch (error) {
      // Fallback to production config
      this.config = await firstValueFrom(this.http.get<AppConfig>('/config.local.json'));
    }
    return this.config;
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
