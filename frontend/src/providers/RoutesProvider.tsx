import { observer } from 'mobx-react-lite';
import React from 'react';
import { FC, ReactElement, useContext } from 'react';
import { Navigate, Route, Routes } from 'react-router-dom';

import { Context } from '..';
import { Role } from '../api';
import AuthPage from '../pages/AuthPage';
import ForbiddenPage from '../pages/ForbiddenPage';
import MainPage from '../pages/MainPage';
import NotFoundPage from '../pages/NotFoundPage';
import ProfilePage from '../pages/ProfilePage';
import Root from '../pages/Root';

interface ForProps {
    for: ReactElement;
}

interface ForWithProps<T> extends ForProps {
    with?: T;
}

const Unauth: FC<ForProps> = observer((props: ForProps) => {
    const { Auth } = useContext(Context);

    if (Auth.isAuth) return <Navigate to={'/'} />;

    return <>{props.for}</>;
});

const Auth: FC<ForWithProps<Role>> = observer((props: ForWithProps<Role>) => {
    const { Auth } = useContext(Context);

    if (Auth.isAuth === false) return <Navigate to={'/auth'} />;

    if (props.with && Auth.roles.includes(props.with) === false) return <ForbiddenPage />;

    return <>{props.for}</>;
});

const RoutesProvider: FC = () => {
    return (
        <Routes>
            <Route path="/" element={<Root />}>
                <Route index element={<MainPage />} />
                <Route path="auth/">
                    <Route index element={<Navigate to={'login'} />} />
                    <Route path="login" element={<Unauth for={<AuthPage tab="login" />} />} />
                    <Route path="register" element={<Unauth for={<AuthPage tab="register" />} />} />
                </Route>
                <Route path="control/">
                    <Route index element={<Navigate to={'connections'} />} />
                    <Route
                        path="connections"
                        element={<Auth with={Role.Administrator} for={<>control connections</>} />}
                    />
                    <Route path="users" element={<Auth with={Role.Administrator} for={<>control users</>} />} />
                    <Route path="servers" element={<Auth with={Role.Administrator} for={<>control servers</>} />} />
                </Route>
                <Route path="tickets/">
                    <Route index element={<Navigate to={'support'} />} />
                    <Route path="support" element={<Auth with={Role.Administrator} for={<>support tickets</>} />} />
                    <Route path="payment" element={<Auth with={Role.PaymentReviewer} for={<>payments tickets</>} />} />
                </Route>
                <Route path="profile" element={<Auth for={<ProfilePage />} />} />
                <Route path="connections" element={<Auth with={Role.Client} for={<>my connections</>} />} />
                <Route path="*" element={<NotFoundPage />} />
            </Route>
        </Routes>
    );
};

export default RoutesProvider;
