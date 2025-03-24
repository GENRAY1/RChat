import {FC, FormEvent, useContext, useState} from "react"

import {
    Container,
    Box,
    Typography,
    TextField,
    Button,
    Grid2,
    CssBaseline,
} from '@mui/material';
import {Link, useNavigate} from "react-router-dom";
import {AuthService} from "../api/auth-service/AuthService.ts";
import AuthContext, {Auth} from "../auth/auth-context.ts";
import {LoginResponse} from "../api/auth-service/auth-contracts.ts";
const LoginPage: FC = () => {

    const [login, setLogin] = useState('');
    const [password, setPassword] = useState('');
    const authContext : Auth = useContext(AuthContext)
    const navigate = useNavigate();
    const handleSubmit = async (e: FormEvent) => {
        e.preventDefault();

        try{
            const loginResponse : LoginResponse
                =  await AuthService.login({login, password});

            if(authContext){
                authContext.login(loginResponse.accessToken)
            }

            navigate("/");
        }catch(error){
            console.log(error);
        }
    };

    return (
        <Container component="main" maxWidth="xs">
            <CssBaseline/>
            <Box
                sx={{
                    marginTop: 8,
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                }}>
                <Typography component="h1" variant="h5">
                    Вход
                </Typography>
                <Box component="form" onSubmit={handleSubmit} noValidate sx={{mt: 1}}>
                    <TextField
                        margin="normal"
                        required
                        fullWidth
                        id="login"
                        label="Логин"
                        name="login"
                        autoComplete="username"
                        autoFocus
                        value={login}
                        onChange={(e) => setLogin(e.target.value)}
                    />
                    <TextField
                        margin="normal"
                        required
                        fullWidth
                        name="password"
                        label="Пароль"
                        type="password"
                        id="password"
                        autoComplete="current-password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                    />
                    <Button
                        type="submit"
                        fullWidth
                        variant="contained"
                        sx={{mt: 3, mb: 2}}>
                        Войти
                    </Button>
                    <Grid2 container justifyContent="flex-end">
                        <Typography variant="body2">
                            Нет аккаунта?{" "}
                            <Link to="/register" style={{textDecoration: 'none'}}>
                                Зарегистрируйтесь
                            </Link>
                        </Typography>
                    </Grid2>
                </Box>
            </Box>
        </Container>
    )
}

export default LoginPage