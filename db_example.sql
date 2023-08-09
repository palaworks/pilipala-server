--
-- PostgreSQL database dump
--

-- Dumped from database version 15.1
-- Dumped by pg_dump version 15.1

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: comment; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.comment (
    comment_id bigint,
    comment_binding bigint,
    user_id bigint,
    comment_body text,
    comment_create_time timestamp without time zone,
    comment_is_reply boolean,
    comment_permission smallint
);


ALTER TABLE public.comment OWNER TO postgres;

--
-- Name: post; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.post (
    post_id bigint,
    post_body text,
    post_create_time timestamp without time zone,
    post_access_time timestamp without time zone,
    post_modify_time timestamp without time zone,
    post_permission smallint,
    post_title text,
    user_id bigint
);


ALTER TABLE public.post OWNER TO postgres;

--
-- Name: user; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."user" (
    user_id bigint,
    user_name text,
    user_pwd_hash text,
    user_create_time timestamp without time zone,
    user_email text,
    user_permission smallint,
    user_access_time timestamp without time zone
);


ALTER TABLE public."user" OWNER TO postgres;

--
-- Data for Name: comment; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.comment (comment_id, comment_binding, user_id, comment_body, comment_create_time, comment_is_reply, comment_permission) VALUES (1001, 1001, 1001, 'Hello World!', '2023-01-09 12:46:56', false, 12);
INSERT INTO public.comment (comment_id, comment_binding, user_id, comment_body, comment_create_time, comment_is_reply, comment_permission) VALUES (1002, 1001, 1001, 'Just a reply.', '2023-01-09 12:47:56', true, 12);


--
-- Data for Name: post; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.post (post_id, post_body, post_create_time, post_access_time, post_modify_time, post_permission, post_title, user_id) VALUES (1001, '欢迎来到 pilipala', '2023-01-09 12:44:47', '2023-01-09 12:44:52', '2023-01-09 12:44:58', 12, 'Welcome to pilipala', 1001);


--
-- Data for Name: user; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."user" (user_id, user_name, user_pwd_hash, user_create_time, user_email, user_permission, user_access_time) VALUES (1001, 'root', '$2a$12$kb/W4EpaH9EnTI4b85/OhelcntMj6TcVk8uo63kTNiJMc/EIW7uG.', '2023-01-09 12:50:03', '', 1023, '2023-01-09 12:50:03');
INSERT INTO public."user" (user_id, user_name, user_pwd_hash, user_create_time, user_email, user_permission, user_access_time) VALUES (1003, 'pl_register', '$2a$12$PdB/S/hHvZ/ixmulLmXfzOh1bZ8ssEkKAWH9/m6r.MoUNhMzmbNIK', '2023-01-09 12:50:06', '', 640, '2023-01-09 12:50:06');
INSERT INTO public."user" (user_id, user_name, user_pwd_hash, user_create_time, user_email, user_permission, user_access_time) VALUES (1002, 'pl_display', '$2a$12$eu.j2USeGDSc.aJhrNTUF.HJz.EZ2/4ZWC6u8UU/5E220d7DrjVzW', '2023-01-09 12:50:04', '', 16, '2023-01-09 12:50:04');


--
-- PostgreSQL database dump complete
--

