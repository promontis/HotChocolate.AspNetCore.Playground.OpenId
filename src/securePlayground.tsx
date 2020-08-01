import React, {useEffect} from "react";
import Playground, {RootState} from "graphql-playground-react";
import {useDispatch, useSelector} from "react-redux";
import {useAuth} from "./useAuth";

export const SecurePlayground = () => {
    const auth = useAuth();
    const state = useSelector<RootState, RootState>((state) => state);
    const dispatch = useDispatch();

    const accessToken = auth ? auth.user?.access_token : "";

    useEffect(() => {
        state.workspaces.forEach((workspace) => {
            const selectedSessionId = workspace.sessions.selectedSessionId;

            workspace.sessions.sessions.forEach((session) => {
                const existingHeaders = session.headers ? JSON.parse(session.headers) : {};

                const newHeaders: any = {
                    ...existingHeaders,
                    Authorization: `Bearer ${accessToken}`
                };

                dispatch({
                    type: "SELECT_TAB",
                    payload: {
                        sessionId: session.id
                    }
                });

                dispatch({
                    type: "EDIT_HEADERS",
                    payload: {
                        headers: JSON.stringify(newHeaders, null, 2)
                    }
                });
            });

            dispatch({
                type: "SELECT_TAB",
                payload: {
                    sessionId: selectedSessionId
                }
            });
        });
    }, [accessToken]);

    const settings = (window as any).Settings;
    const url = settings.url;
    const subscriptionUrl = settings.subscriptionUrl;

    return <Playground endpoint={url} subscriptionEndpoint={subscriptionUrl} />;
};
