import React from "react";
import ReactDOM from "react-dom";
import {BrowserRouter, Switch, Route} from "react-router-dom";
import {Provider} from "react-redux";
import {store} from "graphql-playground-react";
import {makeAuthenticator, makeUserManager, Callback} from "react-oidc";
import {SecurePlayground} from "./securePlayground";

import "graphql-playground-react/build/static/css/index.css";

const settings = (window as any).Settings;
const oidcConfiguration = settings.opendIdConfiguration;
const userManager = makeUserManager(oidcConfiguration);
const PlaygroundWithAuth = makeAuthenticator({userManager: userManager})(SecurePlayground);

ReactDOM.render(
    <Provider store={store}>
        <BrowserRouter>
            <Switch>
                <Route
                    path={`${settings.path}/callback`}
                    render={(routeProps) => (
                        <Callback
                            onSuccess={(user) => {
                                // redirect to playground
                                window.location.href = settings.path;
                            }}
                            userManager={userManager}
                        />
                    )}
                />
                <PlaygroundWithAuth />
            </Switch>
        </BrowserRouter>
    </Provider>,
    document.getElementById("root")
);
