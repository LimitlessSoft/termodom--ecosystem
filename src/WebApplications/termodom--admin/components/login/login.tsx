import { useRef, useState } from 'react'
import { apiPostAsync } from '../../api/api'
import styles from './login.module.css'

export default function Login() {

    const usernameRef = useRef<HTMLInputElement>(null)
    const passwordRef = useRef<HTMLInputElement>(null)

    const [errorStatusLabel, setErrorStatusLabel] = useState<string>('')
    const [successStatusLabel, setSuccessStatusLabel] = useState<string>('')

    return (
        <div className={`grid h-screen fixed w-screen top-0 left-0 text-center`}>
            <div className={`place-self-center px-4 py-2 border-solid border-2 border-zinc-400 rounded-md`}>
                <div className={`font-medium`}>Logovanje</div>
                <div>
                    <Input type={`text`} placeholder={`Korisničko ime`} innerRef={usernameRef} />
                    <Input type={`password`} placeholder={`Lozinka`} innerRef={passwordRef}  />
                    <div className={`w-full p-1`}>
                        <button className={`w-full border-solid bg-td-red text-white font-medium py-2
                            duration-75 ${styles.potvrdiButton}`}
                            onClick={() => {
                                setSuccessStatusLabel('')
                                setErrorStatusLabel('')

                                apiPostAsync('/authenticate', {
                                    username: usernameRef?.current?.value,
                                    password: passwordRef?.current?.value
                                })
                                .then(response => {
                                    if(response.status == 200) {
                                        response.json()
                                        .then((lsResponse: any) => {
                                            if(lsResponse.status == 200) {
                                                setSuccessStatusLabel('Uspešno ste se ulogovali!')

                                                if(!window)
                                                    return

                                                location.reload()
                                                sessionStorage.setItem("bearer_token", lsResponse.payload)
                                            } else {
                                                setErrorStatusLabel('Pogrešno korisničko ime ili lozinka!')
                                            }
                                        })
                                    } else {
                                        setErrorStatusLabel('Pogrešno korisničko ime ili lozinka!')
                                    }
                                })
                                .catch(error => console.error(error))
                            }}>Potvrdi</button>
                    </div>
                    {
                        errorStatusLabel.length == 0 ?
                        '' :
                        <div className={`w-full p-1 text-red-500`}>
                            { errorStatusLabel }
                        </div>
                    }
                    {
                        successStatusLabel.length == 0 ?
                        '' :
                        <div className={`w-full p-1 text-green-500`}>
                            { successStatusLabel }
                        </div>
                    }
                </div>
            </div>
        </div>
    )
}

const Input = (props: any) => {
    return (
        <div>
            <input className={`border-solid border-2 border-slate-200 text-center p-1 m-1`}
                type={props.type}
                placeholder={props.placeholder}
                ref={props.innerRef}/>
        </div>
    )
}