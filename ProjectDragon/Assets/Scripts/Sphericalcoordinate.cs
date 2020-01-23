using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphericalcoordinate : MonoBehaviour
{
    public float rotateSpeed = 1f;
    public float scrollSpeed = 200f;
    // 카메라 움직이는 속도와 휠돌렸을 때 가까워지고 멀어지는 속도

    public Transform pivot;
    // 목표를 포착했다.

    [System.Serializable] // 유니티 상에서 중첩선언된 클래스의 인자에 접근 가능하게
    public class SphericalCoordinates // 중첩 선언 클래스안의 클래스
    {
        public float _radius, _azimuth, _elevation; // 반지름, 방위각, 앙각 

        public float radius
        {
            get { return _radius; }
            private set
            {
                _radius = Mathf.Clamp(value, _minRadius, _maxRadius);
                // 최대 반지름과 최소 반지름 설정
            }
        }

        public float azimuth
        {
            get { return _azimuth; }
            private set
            {
                _azimuth = Mathf.Repeat(value, _maxAzimuth - _minAzimuth);
                // 카메라가 구면 좌표를 따라 좌우로 돌아다니는 데에는 제한이 없어야하므로

            }
        }

        public float elevation
        {
            get { return _elevation; }
            private set
            {
                _elevation = Mathf.Clamp(value, _minElevation, _maxElevation);
                // 앙각에 제한이 없다면 땅을 뚫고 카메라가 들어가거나
                // 물체를 기준으로 앞구르기 하듯 빙글빙글 돌아댕길수 있기에 제한
            }
        }

        public float _minRadius = 3f; // 최소 반지름
        public float _maxRadius = 20f; // 최대 반지름

        public float minAzimuth = 0f; // 최소 방위각
        private float _minAzimuth;

        public float maxAzimuth = 360f; // 최대 방위각
        private float _maxAzimuth;

        public float minElevation = 0f; // 최소 앙각
        private float _minElevation;

        public float maxElevation = 90f; // 최대 앙각
        private float _maxElevation;

        public SphericalCoordinates() { } // 인자없는 생성자

        public SphericalCoordinates(Vector3 cartesianCoordinate)
        { // 벡터(목표) 를 인자로 받은 생성자
            _minAzimuth = Mathf.Deg2Rad * minAzimuth;
            _maxAzimuth = Mathf.Deg2Rad * maxAzimuth;

            _minElevation = Mathf.Deg2Rad * minElevation;
            _maxElevation = Mathf.Deg2Rad * maxElevation;
            // 삼각함수에서 사용하기위해 라디안으로 변환

            radius = cartesianCoordinate.magnitude; // 반지름의 크기
            azimuth = Mathf.Atan2(cartesianCoordinate.z, cartesianCoordinate.x);
            elevation = Mathf.Asin(cartesianCoordinate.y / radius);
            // 방위각, 앙각을 구함
        }

        public Vector3 toCartesian
        {
            get
            {
                float t = radius * Mathf.Cos(elevation);
                // ( r Sinθ CosΦ , r Cos θ , r Sinθ Sin Φ ) 를 기억하자
                return new Vector3(t * Mathf.Cos(azimuth), radius * Mathf.Sin(elevation), t
                                                                     * Mathf.Sin(azimuth));
            }
        }

        public SphericalCoordinates Rotate(float newAzimuth, float newElevation)
        {
            azimuth += newAzimuth;
            elevation += newElevation;
            return this;
            // 움직일 때 그 값을 반영
        }

        public SphericalCoordinates TranslateRadius(float x)
        {
            radius += x;
            return this;
            // 움직일때 그 값을 반영
        }
    }

    public SphericalCoordinates sphericalCoordinates;

    // Use this for initialization
    void Start()
    {
        sphericalCoordinates = new SphericalCoordinates(transform.position);
        transform.position = sphericalCoordinates.toCartesian + pivot.position;
    }

    // Update is called once per frame
    void Update()
    {
        float kh, kv, mh, mv, h, v;
        kh = Input.GetAxis("Horizontal");
        kv = Input.GetAxis("Vertical");

        bool anyMouseButton = Input.GetMouseButton(0) | Input.GetMouseButton(1) |
                                                                      Input.GetMouseButton(2);
        mh = anyMouseButton ? Input.GetAxis("Mouse X") : 0f;
        mv = anyMouseButton ? Input.GetAxis("Mouse Y") : 0f;

        h = kh * kh > mh * mh ? kh : mh;
        v = kv * kv > mv * mv ? kv : mv;

        if (h * h > Mathf.Epsilon || v * v > Mathf.Epsilon)
        {
            transform.position
                = sphericalCoordinates.Rotate(h * rotateSpeed * Time.deltaTime, v *
                                    rotateSpeed * Time.deltaTime).toCartesian + pivot.position;
        }

        float sw = -Input.GetAxis("Mouse ScrollWheel");
        if (sw * sw > Mathf.Epsilon)
        {
            transform.position = sphericalCoordinates.TranslateRadius(sw * Time.deltaTime *
                                                    scrollSpeed).toCartesian + pivot.position;
        }

        transform.LookAt(pivot.position);
    }
}
