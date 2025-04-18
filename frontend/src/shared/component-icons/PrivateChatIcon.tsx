import {FC, SVGProps} from 'react';

const PrivateChatIcon:FC<SVGProps<SVGSVGElement>>  = (props) => {
    return (
        <svg {...props} xmlns="http://www.w3.org/2000/svg" height="1em" fill="currentColor" viewBox="0 0 512 512">
            <path
                d="M 256 256 Q 291 256 320 239 L 320 239 L 320 239 Q 349 222 367 192 Q 384 162 384 128 Q 384 94 367 64 Q 349 34 320 17 Q 291 0 256 0 Q 221 0 192 17 Q 163 34 145 64 Q 128 94 128 128 Q 128 162 145 192 Q 163 222 192 239 Q 221 256 256 256 L 256 256 Z M 210 304 Q 135 306 84 356 L 84 356 L 84 356 Q 34 407 32 482 Q 32 495 41 503 Q 49 512 62 512 L 450 512 L 450 512 Q 463 512 471 503 Q 480 495 480 482 Q 478 407 428 356 Q 377 306 302 304 L 210 304 L 210 304 Z"/>
        </svg>
    );
};

export default PrivateChatIcon;