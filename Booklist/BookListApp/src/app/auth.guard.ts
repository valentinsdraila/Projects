import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable } from "rxjs";

@Injectable({ providedIn: "root" })
class AuthGuard implements CanActivate {
    constructor(private router: Router) { }

    canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
        const token = window.localStorage.getItem("token");
        console.log(token);
        if (token)
            return true
        this.router.navigateByUrl('/auth/login')
        return false
    }
}

export { AuthGuard }