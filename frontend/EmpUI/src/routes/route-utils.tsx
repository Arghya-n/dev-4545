import type { BreadcrumbComponentType } from "use-react-router-breadcrumbs";
import { HomeOutlined } from "@ant-design/icons";
import { useUser } from "@hooks/use-users";

export const DashboardBreadcrumb: BreadcrumbComponentType = () => {
  return <HomeOutlined />;
};

export const DynamicUserBreadcrumb: BreadcrumbComponentType<"id"> = ({
  match,
}) => {
  const { user } = useUser(Number(match.params.id));
  return <div>edit</div>;
};

export const UserDetailsBreadcrumb: BreadcrumbComponentType<"id"> = ({
  match,
}) => {
  const { id } = useUser(match.params);

  return <div>Details</div>;
};
