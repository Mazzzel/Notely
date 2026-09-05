import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

function matchPasswords(): ValidatorFn {
  return (group): ValidationErrors | null => {
    const nouveau = group.get('nouveauMotDePasse')?.value;
    const confirmation = group.get('confirmation')?.value;
    return nouveau === confirmation ? null : { mismatch: true };
  };
}

@Component({
  selector: 'app-change-password',
  imports: [ReactiveFormsModule],
  templateUrl: './change-password.html',
  styleUrl: './change-password.scss'
})
export class ChangePasswordComponent {
  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);

  readonly loading = signal(false);
  readonly errorMessage = signal<string | null>(null);

  form = this.fb.nonNullable.group(
    {
      motDePasseActuel: ['', Validators.required],
      nouveauMotDePasse: ['', [Validators.required, Validators.minLength(8)]],
      confirmation: ['', Validators.required]
    },
    { validators: matchPasswords() }
  );

  submit(): void {
    if (this.form.invalid || this.loading()) return;

    this.loading.set(true);
    this.errorMessage.set(null);

    const { motDePasseActuel, nouveauMotDePasse } = this.form.getRawValue();
    this.auth.changePassword(motDePasseActuel, nouveauMotDePasse).subscribe({
      next: () => {
        this.loading.set(false);
        this.router.navigate(['/accueil']);
      },
      error: (err) => {
        this.loading.set(false);
        this.errorMessage.set(err?.error?.message ?? 'Une erreur est survenue.');
      }
    });
  }
}
