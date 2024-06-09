"use client";

import { signOut, useSession } from "next-auth/react";
import { useEffect, useState } from "react";

import { faRightFromBracket } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  Avatar,
  IconButton,
  ListItem,
  ListItemContent,
  ListItemDecorator,
  Tooltip,
  Typography,
} from "@mui/joy";

import { AgrigateUser } from "@/app/api/auth/[...nextauth]/route";

export default function ProfileButton() {
  const session = useSession();

  const [isClient, setIsClient] = useState(false);

  const handleLogout = () => {
    signOut();
  };

  useEffect(() => {
    setIsClient(true);
  }, []);

  let username = "";
  let initials = "";
  if (isClient && session.data?.user) {
    username =
      session.data.user.name?.trim()?.length ?? 0 > 0
        ? (session.data.user.name as string)
        : (session?.data?.user as AgrigateUser)?.username;

    initials = username.match(/\b(\w)/g)?.join("") ?? "";
  }

  return (
    isClient && (
      <ListItem>
        <ListItemDecorator>
          <Avatar size="lg">{initials}</Avatar>
        </ListItemDecorator>
        <ListItemContent sx={{ marginLeft: "1rem" }}>
          <Typography sx={{ textOverflow: "ellipsis", overflow: "hidden" }}>
            {username}
          </Typography>
          <Tooltip title={session.data?.user?.email}>
            <Typography
              fontSize="xs"
              sx={{ textOverflow: "ellipsis", overflow: "hidden" }}
            >
              {session.data?.user?.email}
            </Typography>
          </Tooltip>
        </ListItemContent>
        <IconButton onClick={handleLogout}>
          <FontAwesomeIcon icon={faRightFromBracket} />
        </IconButton>
      </ListItem>
    )
  );
}
