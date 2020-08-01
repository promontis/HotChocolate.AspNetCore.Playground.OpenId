import {useContext} from "react";
import {AuthenticatorContext, IAuthenticatorContext} from "react-oidc/lib/makeAuth";

export const useAuth = () => {
    return useContext<IAuthenticatorContext | null>(AuthenticatorContext);
};
