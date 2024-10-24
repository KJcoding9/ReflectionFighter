using UnityEngine;


//�v���C���[�̖�󑀍�(�{�[�����ˈʒu����)�X�N���v�g
public class ArrowController : MonoBehaviour
{

    // ��]���x
    public float rotationSpeed = 180f; // 90�x��]�̑��x�i�x���@�j
    //���݂̊p�x�ƈړ�����
    public float currentAngle = 0f;
    // ��]�����i1: ������, -1: �������j
    public int rotationDirection = 1;
    // �p�x�̐���
    private float maxAngle = 80f;
    private float minAngle = -80f;
    // ���S�I�u�W�F�N�g����̋���
    public float distanceFromCenter = 5.0f;
    //���S�I�u�W�F�N�g(�v���C���[�z��)
    public Transform centerObject;

    void Start()
    {
        // �����̉�]��������ɐݒ�i�K�v�ɉ����Ē����j
        transform.rotation = Quaternion.Euler(0, 0, 90);
    }
    void FixedUpdate()
    {
        NewArrowMove();
    }

    void NewArrowMove()
    {
        // �}�E�X�̃X�N���[�����W���擾
        Vector3 mouseScreenPosition = Input.mousePosition;

        // �}�E�X���W�����[���h���W�ɕϊ�
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, Camera.main.nearClipPlane));

        // ���S�I�u�W�F�N�g�ƃ}�E�X�ʒu�̍����x�N�g�����v�Z
        Vector3 direction = mouseWorldPosition - centerObject.position;

        // 2D�Ȃ̂�Z���͖���
        direction.z = 0; 

        // �}�E�X�ʒu�ɑ΂���p�x���v�Z
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // ���̌������}�E�X�̕����ɍ��킹��
        transform.rotation = Quaternion.Euler(0, 0, angle - 90); // ��󂪏����ɂ��邽�߂�90�x�I�t�Z�b�g

        // ���[���h���W�ŋ������v�Z���A���𒆐S�I�u�W�F�N�g�����苗���ɔz�u
        transform.position = centerObject.position + direction.normalized * distanceFromCenter;
    }

    //�����\�b�h(���쐫�������������ߖv)
    //void ArrowMove()
    //{
    //    if (isInitialRotationSet)
    //    {

    //    }
    //    // ����L�[��������Ă���Ԃ̂݉�]���s��
    //    if (Input.GetKey(KeyCode.UpArrow))
    //    {
    //        // ���Ԃɉ����Ċp�x���v�Z
    //        float angle = Time.deltaTime * rotationSpeed * rotationDirection;

    //        // ���݂̊p�x���X�V
    //        currentAngle += angle;

    //        // �p�x�̐����𒴂����ꍇ�A��]�������t�]����
    //        if (currentAngle >= maxAngle)
    //        {
    //            currentAngle = maxAngle;
    //            rotationDirection *= -1;
    //        }
    //        else if (currentAngle <= minAngle)
    //        {
    //            currentAngle = minAngle;
    //            rotationDirection *= -1;
    //        }

    //        // ��]�̒��S�ƂȂ�I�u�W�F�N�g�̎���ŉ�]����
    //        transform.RotateAround(centerObject.position, Vector3.forward, angle);
    //    }
    //}

}
