import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent} from './app/app.component';
import { registerLocaleData } from '@angular/common';
import localeEnZa from '@angular/common/locales/en-ZA';
import { LOCALE_ID, DEFAULT_CURRENCY_CODE } from '@angular/core';
import { appConfig } from './app/app.config';

registerLocaleData(localeEnZa);

bootstrapApplication(AppComponent, {
  ...appConfig,
  providers: [
    ...(appConfig.providers ?? []),
    // added currency and locale 
    { provide: LOCALE_ID, useValue: 'en-ZA' },            
    { provide: DEFAULT_CURRENCY_CODE, useValue: 'ZAR' } 
  ]
}).catch((err) => console.error(err));