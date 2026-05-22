\# UserManager - ASP.NET Core Web API



Dự án \*\*UserManager\*\* là một hệ thống RESTful API cung cấp các chức năng quản lý người dùng, phân quyền và xác thực. Dự án được xây dựng trên nền tảng \*\*.NET 8\*\* và tuân thủ nghiêm ngặt mô hình \*\*Clean Architecture\*\*, kết hợp cùng các Design Pattern phổ biến nhằm đảm bảo tính dễ bảo trì, dễ mở rộng và dễ kiểm thử.



\## 🚀 Công nghệ \& Thư viện sử dụng

\- \*\*Framework:\*\* .NET 8 (ASP.NET Core Web API)

\- \*\*Database:\*\* MongoDB

\- \*\*Architecture:\*\* Clean Architecture

\- \*\*Design Patterns:\*\* CQRS, Mediator Pattern, Repository Pattern

\- \*\*Thư viện chính:\*\*

&#x20; - `MediatR` (Triển khai CQRS \& Mediator)

&#x20; - `MongoDB.Driver` (Giao tiếp cơ sở dữ liệu)

&#x20; - Xác thực JWT (JSON Web Token)

&#x20; - Xử lý mật khẩu an toàn với Hashing



\---



\## 🏗️ Cấu trúc Kiến trúc (Clean Architecture)



Dự án được chia thành 4 lớp (layers) chính, phụ thuộc theo nguyên tắc hướng vào trung tâm (Dependency Rule):



\### 1. 🟡 `UserManager.Domain` (Lớp Cốt lõi)

Nơi chứa các quy tắc nghiệp vụ cốt lõi nhất, không phụ thuộc vào bất kỳ thư viện hay framework ngoại vi nào.

\- \*\*Entities:\*\* `User`, `Role`, `UserProfile`

\- \*\*Constants:\*\* `UserRoles`, `UserStatuses`

\- \*\*Repositories Interfaces:\*\* `IUserRepository`, `IRoleRepository`, `IUserProfileRepository` (Chỉ định nghĩa contract, không triển khai).



\### 2. 🟢 `UserManager.Application` (Lớp Ứng dụng)

Chứa toàn bộ logic ứng dụng (Use cases), điều phối dữ liệu giữa tầng Domain và giao diện.

\- \*\*CQRS (Features):\*\* Phân chia rõ ràng các nghiệp vụ thành `Commands` (Ghi/Sửa/Xóa dữ liệu) và `Queries` (Đọc dữ liệu) bằng `MediatR`. Ví dụ: `Auth`, `Users`, `Admin`.

\- \*\*Pipeline Behaviors:\*\* Middleware xử lý logic cắt ngang như `ValidationBehavior`, `CommandLoggingBehavior`.

\- \*\*Abstractions:\*\* Định nghĩa Interface cho các dịch vụ ngoại vi (`IJwtProvider`, `IPasswordHasher`).



\### 3. 🔵 `UserManager.Infrastructure` (Lớp Hạ tầng)

Nơi triển khai các công nghệ cụ thể và giao tiếp với bên ngoài. Lớp này phụ thuộc vào Application và Domain.

\- \*\*Persistence (MongoDB):\*\* Cấu hình `MongoDbContext`, `MongoDbSettings` và tự động khởi tạo dữ liệu mẫu (`MongoDataSeeder`).

\- \*\*Repositories:\*\* Triển khai thực tế các interface truy xuất dữ liệu MongoDB.

\- \*\*Security:\*\* Triển khai tạo token (`JwtProvider`) và mã hóa mật khẩu (`PasswordHasher`).



\### 4. 🔴 `UserManager.Api` (Lớp Trình bày)

Điểm vào của ứng dụng (Entry point), tiếp nhận các HTTP Request từ client.

\- \*\*Controllers:\*\* Giao tiếp với người dùng qua `AuthController`, `UsersController`, `AdminController`.

\- \*\*Middlewares:\*\* Bắt và xử lý lỗi tập trung qua `GlobalExceptionMiddleware`.

\- \*\*Configuration:\*\* Cấu hình Dependency Injection, Authentication và AppSettings.



\---



\## ✨ Các tính năng chính (Features)



\### 🛡️ Authentication \& Authorization

\- Đăng ký và Đăng nhập.

\- Bảo mật API bằng JWT Bearer Token.

\- Phân quyền truy cập dựa trên Role (`Admin`, `User`).



\### 👤 User Management (Dành cho Role: User)

\- Lấy thông tin hồ sơ cá nhân (Profile).

\- Cập nhật hồ sơ cá nhân (FullName, DateOfBirth, Gender, Address).



\### ⚙️ Admin Management (Dành cho Role: Admin)

\- Lấy danh sách toàn bộ người dùng và hệ thống quyền (Roles).

\- Lọc người dùng theo nhiều tiêu chí (Role, Status, Gender).

\- Tìm kiếm người dùng qua tên hoặc email.

\- Khóa/Mở khóa tài khoản (Cập nhật Status).

\- Phân quyền lại cho người dùng (Cập nhật Role).



\---



\## 🛠️ Hướng dẫn cài đặt và Chạy dự án



\### 1. Yêu cầu hệ thống

\- \[.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

\- \[MongoDB](https://www.mongodb.com/try/download/community) (Đang chạy dưới máy local hoặc URI dạng Cloud - MongoDB Atlas)

\- Visual Studio 2022 hoặc VS Code.



\### 2. Cấu hình Database

Mở file `appsettings.Development.json` hoặc `appsettings.json` tại thư mục `UserManager.Api` và đảm bảo kết nối MongoDB chính xác:

```json

"MongoDbSettings": {

&#x20; "ConnectionString": "mongodb://localhost:27017",

&#x20; "DatabaseName": "UserManagerDb"

}

