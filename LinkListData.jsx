import React, { useState } from "react";
import { Button } from "react-bootstrap";
import propTypes from "prop-types";
import debug from "sabio-debug";
import "./stylelinklistdata.css";
import Modal from "react-bootstrap/Modal";
import { Formik, Form, Field, ErrorMessage } from "formik";
import ListLookUp from "./LinkListLookUp";
import externalLinksService from "services/externalLinksService";
import Swal from "sweetalert2";
// import { tab } from "@testing-library/user-event/dist/types/convenience";

const _logger = debug.extend("LinkListData");

function LinkListData(props) {
  const [show, setShow] = useState(false);

  const [linkData] = useState(props.link);

  const onClickUpdateSubmit = (values) => {
    Swal.fire({
      title: "Are you sure you want to edit this link?",
      showCancelButton: true,
      confirmButtonText: "Update",
      denyButtonText: "Cancel",
    }).then((result) => {
      if (result.isConfirmed) {
        const payload = {
          UrlTypeId: values.urlTypeId.id,
          Url: values.url,
          EntityId: values.entityId,
          EntityTypeId: values.entityTypeId.id,
        };
        _logger("Form submitted with values", values);
        externalLinksService
          .updateExternalLinks(values.id, payload)
          .then(onEditUpdateSuccess)
          .catch(onUpdateError);

        _logger("payload", payload);
      } else if (result.isDenied) {
        Swal.fire("Edit was not Saved", "", "info");
      }
    });
  };

  function onEditUpdateSuccess(response) {
    Swal.fire("Updated", "The link has been updated.", "success");
    _logger("Submission Successful", response);

    //here you call that function you created that was passed as props
  }

  function onUpdateError(error) {
    Swal.fire("Error!", "There was an issue editing the link", "error");
    _logger("Link Form Error", error);
  }

  const handleClose = () => setShow(false);
  const handleShow = () => setShow(true);

  const handleDelete = () => {
    const id = props.link.id;
    Swal.fire({
      title: "Are you sure you want to delete this link?",
      showCancelButton: true,
      confirmButtonText: "Delete",
      denyButtonText: "Cancel",
    }).then((result) => {
      if (result.isConfirmed) {
        externalLinksService
          .deleteExternalLinks(id)
          .then(onDeleteSuccess)
          .catch(onDeleteError);
      } else if (result.isDenied) {
        Swal.fire("Deletion is not saved", "", "info");
      }
    });
  };

  const onDeleteSuccess = () => {
    Swal.fire("Deleted!", "The link has been deleted.", "success");
  };

  const onDeleteError = (error) => {
    _logger("Error deleting link", error);
    Swal.fire("Error!", "There was an issue deleting the link", "error");
  };

  return (
    <tr key={props.link.id}>
      <td>{props.link.urlTypeId.name}</td>
      <td>{props.link.url}</td>
      <td>
        <Button
          variant="outline-primary"
          className="edit-button"
          onClick={handleShow}
        >
          Edit
        </Button>
        <Modal
          show={show}
          onHide={handleClose}
          backdrop="static"
          keyboard={false}
        >
          <Modal.Header>
            <Modal.Title>Edit Link</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            <Formik
              initialValues={linkData}
              onSubmit={onClickUpdateSubmit}
              enableReinitialize={true}
            >
              <Form>
                <label htmlFor="UrlType" className="form-label">
                  <b>Url Type</b>
                </label>
                <Field
                  as="select"
                  name="urlTypeId"
                  className="form-control form-field"
                >
                  <ListLookUp />
                </Field>
                <ErrorMessage
                  name="urlTypeId"
                  component="div"
                  className="has-error"
                />
                <label htmlFor="Url" className="form-label">
                  <b>Url</b>
                </label>
                <Field
                  id="url"
                  name="url"
                  type="text"
                  className="form-control form-field w-50"
                />
                <ErrorMessage
                  name="urlText"
                  component="div"
                  className="has-error"
                />
                <div className="button-group">
                  <Button
                    className="button-group__close"
                    variant="secondary"
                    onClick={handleClose}
                  >
                    Close
                  </Button>
                  <Button
                    className="button-group__sumbit"
                    variant="primary"
                    type="submit"
                  >
                    Save Changes
                  </Button>
                </div>
              </Form>
            </Formik>
          </Modal.Body>
        </Modal>
        <Button
          variant="outline-primary"
          className="delete-button"
          onClick={handleDelete}
        >
          Delete
        </Button>
      </td>
    </tr>
  );
}

LinkListData.propTypes = {
  link: propTypes.shape({
    id: propTypes.string.isRequired,
    urlTypeId: propTypes.string.isRequired,
    url: propTypes.string.isRequired,
    name: propTypes.string.isRequired,
  }).isRequired,
};

export default LinkListData;
1;
