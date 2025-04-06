import React, {useState} from 'react';
import {useNavigate, Link} from 'react-router-dom';
import {
    Container,
    Box,
    Typography,
    TextField,
    Button,
    Grid2,
    CssBaseline,
} from '@mui/material';
import {AccountService} from "../api/account-service/AccountService.ts";
import {ApiErrorData, getApiErrorOrDefault} from "../api/api-error-data.ts";
import {toast} from "react-toastify";
const RegisterPage = () => {
    const [login, setLogin] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const navigate = useNavigate();

    const handleSubmit = async (event: React.FormEvent) => {
        event.preventDefault();

        if (password !== confirmPassword)
            return toast.error("Passwords don't match");

        try {
            await AccountService.register({login, password})

            navigate('/login');
        }
        catch (err){
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
                }}
            >
                <Typography component="h1" variant="h5">
                    Sign up
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
                        autoComplete="new-password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                    />
                    <TextField
                        margin="normal"
                        required
                        fullWidth
                        name="confirmPassword"
                        label="Confirm password"
                        type="password"
                        id="confirmPassword"
                        autoComplete="new-password"
                        value={confirmPassword}
                        onChange={(e) => setConfirmPassword(e.target.value)}
                    />
                    <Button
                        type="submit"
                        fullWidth
                        variant="contained"
                        sx={{mt: 3, mb: 2}}
                    >
                        Sign up
                    </Button>
                    <Grid2 container justifyContent="flex-end">
                        <Typography variant="body2">
                            Already have an account?{' '}
                            <Link to="/login" style={{textDecoration: 'none'}}>
                                Login now
                            </Link>
                        </Typography>
                    </Grid2>
                </Box>
            </Box>
        </Container>
    );
};

export default RegisterPage;