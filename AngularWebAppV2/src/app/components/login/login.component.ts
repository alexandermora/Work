import { Component, OnInit } from '@angular/core';
import { WebServiceClientService } from '../../_services/web-service-client.service';
import { GlobalVarService } from '../../_services/global-var.service';
import { VisibleService } from '../../_services/visible.service';
import { AuthenticationService } from '../../_services/authentication.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  moduleId: module.id,
  templateUrl: './login.component.html'

})
export class LoginComponent implements OnInit {

  model: any = {};
    loading = false;
    error = '';
    auth = false;
    isEmpty = false;
    countries: any = {};
    groups: any = {};
    privileges: any = {};
    timezone: any;
    loginImg = 'http://imagescdn.tweaktown.com/tweakipedia/1/1/113_300_gears-war-tested-dx12-gtx-1080-rx-480-more.jpg';

  constructor(
        private router: Router,
        private authenticationService: AuthenticationService,
        private visibleService: VisibleService,
        private globalVarService: GlobalVarService,
        private webServiceClient: WebServiceClientService
        ) { }

  ngOnInit()
    {
        this.authenticationService.logout();
        this.visibleService.isLogin = false;
        this.visibleService.isAdmin = false;
        this.globalVarService.isUpdated = false;
        this.globalVarService.timezone = Intl.DateTimeFormat().resolvedOptions().timeZone;
        this.webServiceClient.getListWitoutAuth('values/GetCountriesAsync').subscribe(countries =>
        {
            this.countries = countries;
        });
        this.webServiceClient.getListWitoutAuth('values/GetGroupAsync').subscribe(groups =>
        {
            this.groups = groups; // cn=athegreat,cn=AdministratorLocal,ou=Canada,dc=hellsingcorp,dc=com
        });
    }
    login()
    {
        this.loading = true;
        this.isEmpty = (this.model.username === null || this.model.username === '') && (this.model.password === null || this.model.password === '');

        if (this.isEmpty === false)
        {
            this.authenticationService.login(this.model.username, this.model.password, this.model.country, this.model.group).subscribe(result =>
            {
                if (result === true)
                    {
                        this.webServiceClient.getWithAuth('values/GetPrivileges/' + this.model.username).subscribe(p =>
                        {
                            this.privileges = p.data;
                            if (this.privileges === 'Administrator')
                            {
                                this.visibleService.isAdmin = true;
                            }
                            this.visibleService.isLogin = true;
                            this.globalVarService.username = this.model.username;
                            // login successful
                            this.router.navigate(['/']);
                        });
                    }
                    else
                    {
                        this.auth = true;
                        // login failed
                        this.error = 'Username or password is incorrect';
                        this.loading = false;
                    }
            },
            err =>
            {
                this.loading = false;
            });
        }

    }
    logout()
    {
        this.authenticationService.logout();
    }
    onCountryChange()
    {

    }

}
