import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { IConfiguration } from '../model/configuration.model';
import { StorageService } from './storage.service';

import { Observable, Subject } from 'rxjs';

@Injectable()
export class ConfigurationService {
    serverSettings: IConfiguration;
    // observable that is fired when settings are loaded from server
    private settingsLoadedSource = new Subject();
    settingsLoaded$ = this.settingsLoadedSource.asObservable();
    // tslint:disable-next-line:no-inferrable-types
    isReady: boolean = false;

    constructor(private http: HttpClient, private storageService: StorageService) { }

    load() {
        const baseURI = document.baseURI.endsWith('/') ? document.baseURI : `${document.baseURI}/`;
        // tslint:disable-next-line:prefer-const
        let url = `${baseURI}Home/Configuration`;
        this.http.get(url).subscribe((response) => {
            console.log('server settings loaded');
            this.serverSettings = response as IConfiguration;
            console.log(this.serverSettings);
            this.storageService.store('IdentityUrl', this.serverSettings.identityUrl);
            this.isReady = true;
            this.settingsLoadedSource.next();
        });
    }
}
