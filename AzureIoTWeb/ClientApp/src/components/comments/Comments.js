import { useState, useEffect } from "react";
import useHttp from "../../hooks/use-http";
import { useParams } from "react-router-dom";
import classes from "./Comments.module.css";
import NewCommentForm from "./NewCommentForm";
import CommentsList from "./CommentsList";
import { getAllComments } from "../../lib/api";

const Comments = () => {
  const [isAddingComment, setIsAddingComment] = useState(false);
  const { sendRequest, status, data: loadedComments } = useHttp(getAllComments);
  const params = useParams();
  const { quoteId } = params;
  const startAddCommentHandler = () => {
    setIsAddingComment(true);
  };

  const addedCommentHandler = () => {
    setIsAddingComment(false);
    sendRequest(quoteId);
  };
  useEffect(() => {
    sendRequest(quoteId);
  }, [sendRequest, quoteId]);

  return (
    <section className={classes.comments}>
      <h2>User Comments</h2>
      {!isAddingComment && (
        <button className="btn" onClick={startAddCommentHandler}>
          Add a Comment
        </button>
      )}
      {isAddingComment && <NewCommentForm onAddComment={addedCommentHandler} />}
      {(!loadedComments || loadedComments.length === 0) &&
        status === "completed" && (
          <div className="centered">
            <p>No Comments yet</p>
          </div>
        )}
      {(loadedComments) && status === "completed" && (
        <CommentsList comments={loadedComments} />
      )}
    </section>
  );
};

export default Comments;
