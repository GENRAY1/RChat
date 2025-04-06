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
import AuthContext, {AuthContextValue} from "../features/auth/AuthContext.ts";
import {LoginResponse} from "../api/account-service/account-contracts.ts";
import {AccountService} from "../api/account-service/AccountService.ts";
import {AxiosResponse} from "axios";
import {toast} from "react-toastify";
import {ApiErrorData, getApiErrorOrDefault} from "../api/api-error-data.ts";

const LoginPage: FC = () => {
    const [login, setLogin] = useState('');
    const [password, setPassword] = useState('');
    const authContext : AuthContextValue = useContext(AuthContext)
    const navigate = useNavigate();
    const handleSubmit = async (e: FormEvent) => {
        e.preventDefault();

        try{
            const loginResponse : AxiosResponse<LoginResponse>
                =  await AccountService.login({login, password});

            authContext.login(loginResponse.data.accessToken)
            navigate("/");
        }catch(err){
            const errorData:ApiErrorData = getApiErrorOrDefault(err)
            toast.error(errorData.Message)
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
                    Login
                </Typography>
                <Box component="form" onSubmit={handleSubmit} noValidate sx={{mt: 1}}>
                    <TextField
                        margin="normal"
                        required
                        fullWidth
                        id="login"
                        label="Login"
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
                        label="Password"
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
                        Login
                    </Button>
                    <Grid2 container justifyContent="flex-end">
                        <Typography variant="body2">
                            You don't have an account?{" "}
                            <Link to="/register" style={{textDecoration: 'none'}}>
                                Sign up
                            </Link>
                        </Typography>
                    </Grid2>
                </Box>
            </Box>
        </Container>
    )
}

export default LoginPage