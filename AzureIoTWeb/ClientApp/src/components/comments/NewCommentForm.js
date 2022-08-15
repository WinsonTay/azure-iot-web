import { useEffect, useRef } from "react";
import { useParams } from "react-router-dom";
import useHttp from "../../hooks/use-http";
import { addComment } from "../../lib/api";
import classes from "./NewCommentForm.module.css";

const NewCommentForm = (props) => {
  const commentTextRef = useRef();
  const params = useParams();
  const { quoteId } = params;

  const { sendRequest, status } = useHttp(addComment);
  const { onAddComment } = props;
  const submitFormHandler = (event) => {
    event.preventDefault();

    // optional: Could validate here
    const comment = commentTextRef.current.value;
    // send comment to server
    sendRequest({
      quoteId: quoteId,
      commentData: {text:comment},
    });
  };
  useEffect(() => {
    if (status === "completed") {
      onAddComment();
    }
  }, [status, onAddComment]);

  return (
    <form className={classes.form} onSubmit={submitFormHandler}>
      <div className={classes.control} onSubmit={submitFormHandler}>
        <label htmlFor="comment">Your Comment</label>
        <textarea id="comment" rows="5" ref={commentTextRef}></textarea>
      </div>
      <div className={classes.actions}>
        <button className="btn">Add Comment</button>
      </div>
    </form>
  );
};

export default NewCommentForm;
