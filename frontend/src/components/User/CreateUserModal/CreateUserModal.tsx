import {FC, FormEvent, useState} from 'react';
import styles from './CreateUserModal.module.css'
import {AxiosResponse, HttpStatusCode} from "axios";
import {User} from "../../../models/user/User.ts";
import {UserService} from "../../../api/user-service/UserService.ts";


interface CreateUserModalProps{
    setUser: (user: User) => void;
}

const CreateUserModal : FC<CreateUserModalProps> = ({setUser}) => {
    const [firstname, setFirstName] = useState<string>('');
    const [lastname, setLastname] = useState<string | undefined>(undefined);
    const [username, setUsername] = useState<string | undefined>(undefined);
    const [description, setDescription] = useState<string | undefined>(undefined);
    const [dateOfBirth, setDateOfBirth] = useState<Date | undefined>(undefined);
    const  handleSubmit = async (e : FormEvent) => {
        e.preventDefault();

        if(firstname.length < 1)
            return alert('Please enter a valid first name');

        const createUserResponse : AxiosResponse<User> =
            await UserService.Create({firstname, lastname, username, description, dateOfBirth});

        if(createUserResponse.status !== HttpStatusCode.Ok)
            return alert(createUserResponse.statusText)

        setUser(createUserResponse.data)
    }

    return (
        <div className={styles.modal}>
            <div className={styles.modalContent}>
                <form onSubmit={handleSubmit}>
                    <label className={styles.formLabel}>
                        Firstname
                        <input
                            type="text"
                            value={firstname}
                            onChange={(e) => setFirstName(e.target.value)}
                            required
                            className={styles.formInput}
                        />
                    </label>
                    <label className={styles.formLabel}>
                        Lastname:
                        <input
                            type="text"
                            value={lastname}
                            onChange={(e) => setLastname(e.target.value)}
                            className={styles.formInput}
                        />
                    </label>
                    <label className={styles.formLabel}>
                        Username:
                        <input
                            type="text"
                            value={username}
                            onChange={(e) => setUsername(e.target.value)}
                            className={styles.formInput}
                        />
                    </label>
                    <label className={styles.formLabel}>
                        Description:
                        <textarea
                            value={description}
                            onChange={(e) => setDescription(e.target.value)}
                            className={styles.formTextarea}
                        />
                    </label>
                    <label className={styles.formLabel}>
                        Date of Birth:
                        <input
                            type="date"
                            value={dateOfBirth?.toString()}
                            onChange={(e) => setDateOfBirth(new Date (e.target.value))}
                            className={styles.formInput}
                        />
                    </label>
                    <button type="submit" className={styles.formButton}>
                        Complete registration
                    </button>
                </form>
            </div>
        </div>
    );
};

export default CreateUserModal