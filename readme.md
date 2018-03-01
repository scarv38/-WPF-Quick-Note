BÁO CÁO BÀI TẬP CUỐI KÌ
================


QUICK NOTE
-----------------
#### Thông tin cá nhân
- Họ tên: Võ Hữu Thắng
- MSSV: 1512526
- Email: thangvo8797@gmail.com

#### Các chức năng làm được
##### Màn hình chính (View Note)
- Nhấp vào tag sẽ hiện ra các note (gồm tựa đề, thời gian chỉnh sửa lần cuối và nội dung) chứa tag đó ở cột Notes.
- Nhấp vào mỗi note sẽ hiện ra tiêu đề, nội dung của note đó ở cột Edit Note.
- Khi chỉnh sửa nội dung note:
-- Bấm vào nút Save, chương trình sẽ cập nhập nội dung mới vào database.
-- Bấm vào nút Delete, chương trình sẽ xóa note đó và cập nhập nội dung vào database.
- Các phím chức năng:
--  Add Note (mở màn hình Thêm Note)
-- View Statistics (xem thống kê các tag)
-- About (thông tin chương trình) 
-- Exit (thoát hẳn chương trình).
- Khi bấm nút thu nhỏ hoặc nút thoát, màn hình chính sẽ tự động ẩn đi, hiện thông báo ở notify icon trên taskbar.
- Splitter giữa 3 cột cho phép thay đổi kích thước các cột.

##### Màn hình thêm note mới (Add Note)
- Nhập tiêu đề, nội dung note và danh sách tag (mỗi tag cách nhau bởi dấu ',') và bấm Save để chương trình kiểm tra điều kiện và lưu note và database.

##### Màn hình thống kê thông tin tag (Statistics)
- Hiện thị danh sách các tag với dạng tag cloud. Rê chuột vào tag tương ứng để biết số lần sử dụng của tag đó.

##### Hook và Notify Icon
- Khi bấm tổ hợp phím Shift + F6, sẽ tự động mở màn hình Add Note.
- Chương trình hiện một icon trên taskbar, bấm chuột phải vào icon sẽ hiện ra menu với 4 lựa chọn:
-- Open: mở màn hình chính, có thể double click vào icon để thực hiện tính năng này.
-- Add Note: mở màn hình Add Note.
-- View Statistics: mở bảng thống kê.
--Close: đóng chương trình.

#### Các chức năng nâng cao
- Tạo bảng thống kê các tag bằng tag cloud thay vì vẽ biểu đồ. Kích cỡ các tag được tính theo số lần xuất hiện của tag.
- Save thông tin của tag và note trên file content.dtb. Vốn là 1 file xml, sử dụng linq để truy xuất dữ liệu.
- Ràng buộc sao cho chỉ có một instance của chương trình được mở một lúc.

#### Các luồng sự kiện
##### Luồng sự kiện chính
- Chạy chương trình, dữ liệu sẽ được load từ database vào (nếu có).
- Bấm tổ hợp phím Shift+F6, hiện màn hình Add Note.
- Nhập thông tin note, bấm Save, kiểm tra điều kiện hợp lệ. Lưu ý: các dấu cách trong chuỗi tag sẽ tự động bị xóa.
- Cập nhập note và tag mới tạo vào danh sách tag và note ở cửa số chính, cập nhập thống kê cho tag, lưu thông tin vào database.

##### Luồng sự kiện phụ
- Bỏ trống ít nhất một trong ba ô khi thêm note => Thông báo lỗi.
- Danh sách tag nhập không đúng cú pháp:
-- TH1: Người dùng nhập vào chuỗi ",," => kết quả không có tag nào được nhập => Thông báo lỗi.
-- TH2: Người dùng nhập "tag1," hoặc "tag1,,tag2" => kết quả chỉ có tag1 và tag2 được thêm vào.
- Người dùng tự ý chỉnh sửa file database trong lúc chạy:
-- TH1: Khi người dùng thêm note mới: Xóa hết dữ liệu, tạo file xml rỗng với root element và cập nhập lại dữ liệu mới nhập.
-- TH2: Khi người dùng chỉnh sửa / xóa note: hiện thông báo lỗi.
- Khi mở chương trình và file database bị lỗi => Xóa hết dữ liệu và tạo file xml rỗng với root element

#### Link bitbucket
```
https://bitbucket.org/scarlet38/final_quicknote
```

#### Link Youtube
https://youtu.be/McOq5NVoXXI

KẾT THÚC BÁO CÁO
-----------------