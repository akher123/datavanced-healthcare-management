import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { provideHttpClient } from '@angular/common/http';
import { API_BASE_URL } from './config/api.config';

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ],
  providers: [
    provideHttpClient(),
    {
      provide: API_BASE_URL,
      useValue: 'https://localhost:7024'
    }
  ]
})
export class CoreModule { }
