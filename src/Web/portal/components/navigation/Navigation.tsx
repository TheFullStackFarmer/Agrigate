"use client";

import {
  faRightFromBracket,
  faSquarePollVertical,
} from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  Avatar,
  IconButton,
  List,
  ListItem,
  ListItemButton,
  ListItemContent,
  ListItemDecorator,
  Tooltip,
  Typography,
} from "@mui/joy";
import { usePathname } from "next/navigation";

export default function Navigation() {
  const pathname = usePathname();

  return (
    <div className="flex flex-col h-full">
      <Typography className="text-center" fontSize="xl">
        Agrigate
      </Typography>
      <List>
        <ListItem>
          <ListItemButton selected={pathname === "/"} className="rounded-2xl">
            <ListItemDecorator>
              <FontAwesomeIcon icon={faSquarePollVertical} />
            </ListItemDecorator>
            <ListItemContent>
              <Typography>Dashboard</Typography>
            </ListItemContent>
          </ListItemButton>
        </ListItem>
      </List>
      <div className="grow" />
      <ListItem>
        <ListItemDecorator>
          <Avatar size="lg">KW</Avatar>
        </ListItemDecorator>
        <ListItemContent sx={{ marginLeft: "1rem" }}>
          <Typography sx={{ textOverflow: "ellipsis", overflow: "hidden" }}>
            Kevin Williams
          </Typography>
          <Tooltip title="kevin.williams@thefullstackfarmer.com">
            <Typography
              fontSize="xs"
              sx={{ textOverflow: "ellipsis", overflow: "hidden" }}
            >
              kevin.williams@thefullstackfarmer.com
            </Typography>
          </Tooltip>
        </ListItemContent>
        <IconButton>
          <FontAwesomeIcon icon={faRightFromBracket} />
        </IconButton>
      </ListItem>
    </div>
  );
}
