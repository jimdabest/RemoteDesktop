# Remote Desktop Application

Đây là một ứng dụng điều khiển máy tính từ xa (Client-Server) nguyên bản dành cho Windows, được xây dựng bằng **.NET 8.0** và **Windows Forms**. Dự án sử dụng kiến trúc mạng giao thức kép (dual-protocol) để cân bằng giữa độ tin cậy và hiệu năng, cho phép điều chỉnh chất lượng hình ảnh linh hoạt nhằm thích ứng với điều kiện băng thông.

## Tính năng nổi bật

* **Xác thực bảo mật:** Kết nối được bảo vệ bằng mã PIN (người dùng tự đặt hoặc hệ thống tự động sinh ra).
* **Điều khiển toàn diện:** Hỗ trợ đầy đủ các thao tác bao gồm di chuyển chuột, click trái/phải, cuộn chuột và gõ phím.
* **Chế độ quan sát (View Only):** Tùy chọn chỉ xem màn hình mà không can thiệp vào máy chủ.
* **Tùy biến hiển thị và hiệu năng:** Điều chỉnh chất lượng hình ảnh (Image Quality) từ 10% đến 100%.
  * Cấu hình độ nén băng thông (Low, Medium, High).
  * Hỗ trợ đa dạng chế độ hiển thị: Stretch, Zoom, Center, Normal và chế độ Toàn màn hình (Fullscreen).
* **Ghi log hệ thống:** Máy chủ lưu lại chi tiết lịch sử kết nối, ngắt kết nối và các thay đổi trạng thái.


---
## Hướng dẫn cài đặt và sử dụng

### 1. Khởi chạy Server (Máy cần bị điều khiển)
1. Mở ứng dụng, tại màn hình Launcher chọn **CHO PHÉP ĐIỀU KHIỂN (Run as Server)**.
2. Cấu hình Port (mặc định là `5000`).
3. Nhập mã PIN mong muốn (nếu để trống, hệ thống sẽ tự động tạo một mã PIN ngẫu nhiên gồm 4 chữ số).
4. Nhấn **Start**. Trạng thái sẽ chuyển sang *Online* và ứng dụng bắt đầu lắng nghe kết nối.

### 2. Khởi chạy Client (Máy thực hiện điều khiển)
1. Trên một máy tính khác, mở ứng dụng và chọn **ĐIỀU KHIỂN MÁY KHÁC (Run as Client)**.
2. Tại mục kết nối, nhập địa chỉ **IP** của máy Server, **Port** và **Mã PIN**.
3. Nhấn **Connect**. 
4. Nếu mã PIN chính xác, màn hình của máy chủ sẽ lập tức xuất hiện. Bạn có thể sử dụng nút **Settings** để tinh chỉnh chất lượng hình ảnh cho phù hợp với tốc độ mạng.

---

## Kỹ thuật Cốt lõi (Interesting Techniques)

* **Mạng Giao thức kép (Dual-Protocol Networking):** Tận dụng các socket TCP bất đồng bộ (`BeginConnect`, `BeginReceive`) để đảm bảo truyền tải tuyệt đối chính xác các lệnh điều khiển ngoại vi và xác thực. Đồng thời, luồng truyền phát màn hình được đưa qua giao thức UDP (`BeginReceiveFrom`, `SendTo`) nhằm giảm thiểu độ trễ tối đa (chấp nhận rớt khung hình để tránh hiện tượng giật lag do chờ đợi).
* **Băm Khung hình Delta (Delta Frame Hashing):** Hệ thống tính toán mã băm MD5 cho từng khung hình vừa chụp. Nếu mã băm trùng khớp với khung hình trước đó (màn hình không có chuyển động), hệ thống sẽ bỏ qua việc truyền tải, giúp tiết kiệm đáng kể tài nguyên mạng.
* **Chia nhỏ Gói tin Tùy chỉnh (Custom Packet Chunking):** Để vượt qua giới hạn khắt khe về kích thước payload của UDP, `ScreenCaster` tự động băm nhỏ các ảnh JPEG nén thành nhiều mảnh (`ImageChunkPacket`). Phía Client (`ScreenReceiver`) sẽ thu thập vào bộ đệm và tiến hành nội suy, lắp ráp lại dựa trên ID khung hình trước khi kết xuất lên UI.
* **Ánh xạ Tọa độ Động (Dynamic Coordinate Mapping):** Client sẽ tự động tính toán tỷ lệ khung hình (aspect ratios) và hệ số chia dãn. Điều này đảm bảo hệ tọa độ chuột trên phần mềm luôn được ánh xạ chính xác tới độ phân giải thực của máy chủ từ xa, bất kể người dùng đang sử dụng chế độ hiển thị "Zoom" hay "Stretch".

---

## Công nghệ & Thư viện Tiêu biểu
Hệ thống được chia làm hai module chính: **FormServer** (Máy bị điều khiển) và **FormClient** (Máy điểu khiển). Để cân bằng giữa độ tin cậy và hiệu năng, dự án sử dụng hai giao thức mạng song song:

| Giao thức | Chức năng chính | Đặc điểm |
| :--- | :--- | :--- |
| **TCP** | Truyền lệnh (CommandPacket) | Đảm bảo xác thực PIN và các lệnh điều khiển (chuột, phím) không bị thất thoát hoặc sai lệch. |
| **UDP** | Truyền phát màn hình (ImageChunkPacket) | Gửi các khung hình đã được chia nhỏ (chunks) với tốc độ cao, bỏ qua các gói tin rớt để tối ưu độ trễ. |
* **P/Invoke (Platform Invocation Services):** Server móc nối trực tiếp (hook) vào Windows API không quản lý (`user32.dll`) để mô phỏng tín hiệu phần cứng cấp thấp. Cụ thể, `mouse_event` được dùng để định vị/nhấp chuột tuyệt đối, và `keybd_event` để tiêm thao tác gõ phím.
* **System.Drawing.Common:** Được ứng dụng chuyên sâu cho việc chụp toàn bộ màn hình vào bộ nhớ RAM (`CopyFromScreen`) và mã hóa nén (JPEG encoder) trực tiếp theo thời gian thực.
* **Tối ưu hóa UI/UX:** Giao diện sử dụng font Arial làm chuẩn đồng nhất, riêng phần xuất Log của Server sử dụng font monospaced (Courier New) để đảm bảo việc căn lề dễ đọc.

---

## Cấu trúc Dự án

```plaintext
/
├── RemoteDesktop/
├── .gitignore
└── RemoteDesktop.slnx
```

**RemoteDesktop**: Chứa tất cả WinForms UI code (`FormClient.cs`, `FormServer.cs`), custom network packet models (`CommandPacket.cs`, `ImageChunkPacket.cs`), và streaming logic trong (`ScreenCaster.cs`, `ScreenReceiver.cs`).