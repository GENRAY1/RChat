import {FC, useContext} from "react";
import {Container, Grid2} from "@mui/material";
import AuthContext, {Auth} from "../auth/auth-context.ts";

const MainPage: FC = () => {
    const authContext : Auth = useContext(AuthContext)

    return (
        <Container>
            <Grid2 container>
                {JSON.stringify(authContext.user, null, 2)}
            </Grid2>
        </Container>
    )
}

export default MainPage;