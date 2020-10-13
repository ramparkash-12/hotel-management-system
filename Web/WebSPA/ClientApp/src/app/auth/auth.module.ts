import { NgModule } from '@angular/core';

import { AuthLayoutComponent } from './auth-layout.component';
import { AuthLayoutRouting } from './auth-layout.routing';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { VerifyEmailComponent } from './verify-email/verify-email.component';
import { ConfirmEmailComponent } from './confirm-email/confirm-email.component';

@NgModule({
  declarations: [AuthLayoutComponent, LoginComponent, RegisterComponent, VerifyEmailComponent, ConfirmEmailComponent],
  imports: [AuthLayoutRouting]
})
export class AuthModule { }
