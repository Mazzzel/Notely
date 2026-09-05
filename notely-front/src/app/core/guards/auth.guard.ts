import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { map } from 'rxjs';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = () => {
  const auth = inject(AuthService);
  const router = inject(Router);

  const user = auth.currentUser();
  if (user) {
    return user.doitChangerMotDePasse ? router.parseUrl('/change-password') : true;
  }

  return auth.me().pipe(
    map((u) => {
      if (!u) return router.parseUrl('/login');
      return u.doitChangerMotDePasse ? router.parseUrl('/change-password') : true;
    })
  );
};

export const authenticatedGuard: CanActivateFn = () => {
  const auth = inject(AuthService);
  const router = inject(Router);

  if (auth.currentUser()) return true;

  return auth.me().pipe(map((u) => (u ? true : router.parseUrl('/login'))));
};

export const guestGuard: CanActivateFn = () => {
  const auth = inject(AuthService);
  const router = inject(Router);

  const user = auth.currentUser();
  if (user) {
    return router.parseUrl(user.doitChangerMotDePasse ? '/change-password' : '/accueil');
  }

  return auth.me().pipe(
    map((u) => {
      if (!u) return true;
      return router.parseUrl(u.doitChangerMotDePasse ? '/change-password' : '/accueil');
    })
  );
};
